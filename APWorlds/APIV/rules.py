from __future__ import annotations
from typing import TYPE_CHECKING
from worlds.generic.Rules import set_rule

if TYPE_CHECKING:
    from .world import GTAIVWorld

def set_all_rules(world: GTAIVWorld) -> None:
    set_all_entrance_rules(world)
    set_all_location_rules(world)
    set_completion_condition(world)

def set_all_entrance_rules(world: GTAIVWorld) -> None:
    # Island Unlocks (These refer to the Entrances created in regions.py)
    set_rule(world.get_entrance("Bridge to Algonquin"), 
             lambda state: state.has("Blow Your Cover", world.player))
    set_rule(world.get_entrance("Bridge to Alderney"), 
             lambda state: state.has("Three Leaf Clover", world.player))

def set_all_location_rules(world: GTAIVWorld) -> None:
    # Weapon requirements for hard missions
    set_rule(world.get_location("Three Leaf Clover"),
             lambda state: state.has_any({"Assault Rifle", "Carbine Rifle"}, world.player))

def set_completion_condition(world: GTAIVWorld) -> None:
    # Victory logic
    world.multiworld.completion_condition[world.player] = lambda state: \
        state.has("Out of Commission", world.player) or state.has("A Revenger's Tragedy", world.player)