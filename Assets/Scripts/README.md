# Scripts

## Layers
There are several different layers in this project. They are as follows:
* Layer 8: Player - should be used for objects related to the player entity.
* Layer 9: Shield - should be used for objects related to the player's shield.
* Layer 10: Enemy Bullets - should be used for all projectiles originating from enemies.
* Layer 11: Player Bullets - should be used for all projectiles originating from the player.

## Boss setup
Bosses are assembled from prefabs. Create a standard enemy object that you want to serve as the boss.
Attach a `Boss` script to it, and specify `attacks`' length to be the number of attacks the boss will perform.
Fill the `EmitterPrefabs` field of `attacks` with emitter prefabs. Save the resulting `BossAttack` as a 
prefab in the `BossAttacks` resource folder.
