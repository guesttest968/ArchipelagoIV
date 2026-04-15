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
    # Algonquin -> Broker requires reaching the Corner Kids location
    set_rule(world.get_entrance("Bridge to Broker"), 
             lambda state: state.can_reach("Corner Kids", "Location", world.player))
             
    # Broker -> Alderney requires reaching the This Ain't Checkers location
    set_rule(world.get_entrance("Bridge to Alderney"), 
             lambda state: state.can_reach("This Ain't Checkers", "Location", world.player))

def set_all_location_rules(world: GTAIVWorld) -> None:
    # Weapon requirements for hard missions (This stays the same!)
    set_rule(world.get_location("Party's Over"),
             lambda state: state.has_any({"Assault Rifle", "Carbine Rifle"}, world.player))

def set_completion_condition(world: GTAIVWorld) -> None:
    # Victory logic checks if the final mission location can be reached
    world.multiworld.completion_condition[world.player] = lambda state: \
        state.can_reach("Departure Time", "Location", world.player)