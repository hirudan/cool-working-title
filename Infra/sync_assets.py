import os
import sys
import glob
import json
import shutil
import hashlib
import argparse
import subprocess as sp
from datetime import datetime

SCRIPT_PATH = os.path.dirname(os.path.realpath(__file__))

CHECK_FILES = [
    "*.3dm",
    "*.3ds",
    "*.blend",
    "*.c4d",
    "*.collada",
    "*.dae",
    "*.dxf",
    "*.fbx",
    "*.jas",
    "*.lws",
    "*.lxo",
    "*.ma",
    "*.max",
    "*.mb",
    "*.obj",
    "*.ply",
    "*.skp",
    "*.stl",
    "*.ztl",
    "*.aif",
    "*.aiff",
    "*.it",
    "*.mod",
    "*.mp3",
    "*.ogg",
    "*.s3m",
    "*.wav",
    "*.xm",
    "*.otf",
    "*.ttf",
    "*.bmp",
    "*.exr",
    "*.gif",
    "*.hdr",
    "*.iff",
    "*.jpeg",
    "*.jpg",
    "*.pict",
    "*.png",
    "*.psd",
    "*.tga",
    "*.tif",
    "*.tiff"
]

SERVER_LOC = "Z:\\assets"
CHECK_DIR = os.path.join(SCRIPT_PATH, "..")
DATE_STR_FORMAT = "%Y-%m-%d %H:%M:%S"
CHECK_FILE = os.path.join(SCRIPT_PATH, "..", "asset_changes.json")

# Helpers #
def get_current_branch():
    return sp.check_output("git rev-parse --abbrev-ref HEAD").decode("utf-8").strip()

def hash_file(fpath):
    hasher = hashlib.md5()
    with open(fpath, 'rb') as afile:
        buf = afile.read()
        hasher.update(buf)
    return hasher.hexdigest()

def json_serialize(o):
    if isinstance(o, datetime):
        return o.strftime(DATE_STR_FORMAT)

def json_parse(dct):
    for k, v in dct.items():
        if k != "time":
            continue

        try:
            dct[k] = datetime.strptime(v, DATE_STR_FORMAT)
        except:
            pass

    return dct

def get_hashes_only(file):
    return {fpath: {"hash": dct["hash"]} for fpath, dct in file.items()}

def get_dates_only(file):
    return {fpath: {"time": dct["time"]} for fpath, dct in file.items()}

def generate_hash_commits(files):
    hashes = {}

    for fpath in files:
        hashes[fpath] = {"hash": hash_file(fpath), "time": datetime.now()}

    return hashes

def write_hash_checklist(hashes):
    with open(CHECK_FILE, "w+") as f:
        json.dump(hashes, f, default=json_serialize, indent=2)

def get_prior_hash_checklist(cur_branch, to_branch):
    try:
        if cur_branch != to_branch:
            sp.check_call("git fetch origin")
            sp.check_call("git checkout {} -- asset_changes.json".format(to_branch), cwd=CHECK_DIR)

        if not os.path.exists(CHECK_FILE):
            with open(CHECK_FILE, "w") as f:
                f.write("{}")
            return {}

        with open(CHECK_FILE, "r") as f:
            dct = json.load(f, object_hook=json_parse)
    
            # convert relative to absolute
            return {os.path.join(CHECK_DIR, k): v for k, v in dct.items()}
    finally:
        if cur_branch != to_branch:
            sp.check_call("git checkout {} -- asset_changes.json".format(cur_branch), cwd=CHECK_DIR)

def get_syncable_files():
    assets = []
    for ext in CHECK_FILES:
        for file in glob.glob(os.path.join(CHECK_DIR, "Assets", "**", ext), recursive=True):
            assets.append(file)
    return assets

def calculate_time_diff(dict_one, dict_two):

    time_diff = {}
    for k, v in dict_one.items():
        if k in dict_two:
            if dict_one[k]["hash"] == dict_two[k]["hash"]:
                continue

            time_diff[k] = dict_one[k]["time"] >= dict_two[k]["time"]
        else:
            time_diff[k] = True

    for k, v in dict_two.items():
        if k in dict_one:
            continue

        # Setting to false means we have an "older" file, non-existent
        # force a download
        time_diff[k] = False

    return time_diff

def main():
    parser = argparse.ArgumentParser(description="Sync assets")
    parser.add_argument("--to-branch", "-b", help="Branch to push and pull assets to. Defaults to current branch", default=get_current_branch())
    parser.add_argument("--folder-location", "-f", help="Folder to push and pull data from.", default=SERVER_LOC)
    parser.add_argument("--yes", help="Yes to prop push.", action="store_true", default=False)
    parser.add_argument("--no", help="No to prop push.", action="store_true", default=False)
    args = parser.parse_args()

    # Do not run script if there are files that need to be registered in a commit first
    check_status = sp.check_output("git status").decode("utf-8")
    if "asset_changes.json" in check_status:
        print("Please commit asset_changes.json locally first.")
        return -1

    syncables = get_syncable_files()
    check_sums = generate_hash_commits(syncables)
    cur_branch = get_current_branch()
    to_branch = args.to_branch
    push_location = os.path.join(args.folder_location, to_branch)
    prior_check_sums_in_git = get_prior_hash_checklist(cur_branch, to_branch)

    # This happens if a new branch is created. Check server remote and see if there are any files there, if not, wipe everything.
    if not os.path.exists(push_location):
        prior_check_sums_in_git = {}

    # check if checksums are the same
    if get_hashes_only(check_sums) == get_hashes_only(prior_check_sums_in_git):
        print("No asset files needed to sync.")
        return

    # There is a difference! Check if the prior is older, if so we can push
    time_diff = calculate_time_diff(check_sums, prior_check_sums_in_git)

    newer_files = []
    older_files = []
    # Use relative path version
    final_hash_file = {os.path.relpath(k, CHECK_DIR): v for k, v in check_sums.items()}
    for fpath, comp in time_diff.items():
        if comp:
            newer_files.append(fpath)
        else:
            older_files.append(fpath)
            final_hash_file[os.path.relpath(fpath, CHECK_DIR)] = prior_check_sums_in_git[fpath]


    rel_newer_files = [os.path.relpath(v, CHECK_DIR) for v in newer_files]
    rel_older_files = [os.path.relpath(v, CHECK_DIR) for v in older_files]

    print("From branch: {}".format(cur_branch))
    print("To branch: {}".format(to_branch))
    print()
    print("Relative to project root")
    print("New files to be uploaded to asset server:\n {} ".format(rel_newer_files))
    print("Old files that need to be pulled from asset server:\n {}".format(rel_older_files))
    print()
    if not args.no:
        print("Files will be pushed to {}".format(push_location))

    user_input = None

    if args.no:
        return -1
    elif not args.yes:
        user_input = input("Y to confirm. Anything else to abort. ")
    else:
        user_input = "Y"

    if user_input != "Y":
        print("ABORTING...")
        return -1


    # Upload the new files
    if not os.path.exists(push_location):
        os.makedirs(push_location)

    for idx, fpath in enumerate(rel_newer_files):
        final_loc = os.path.join(push_location, fpath)
        print("Writing... {} to {}".format(fpath, final_loc))

        os.makedirs(os.path.dirname(final_loc), exist_ok=True)
        shutil.copyfile(newer_files[idx], final_loc)

    # Download the files to replace old files
    for idx, fpath in enumerate(rel_older_files):
        final_loc = os.path.join(push_location, fpath)
        print("Writing... {} to {}".format(final_loc, fpath))

        os.makedirs(os.path.dirname(older_files[idx]), exist_ok=True)
        shutil.copyfile(final_loc, older_files[idx])

    write_hash_checklist(final_hash_file)

if __name__ == "__main__":
    main()
