from __future__ import annotations
from typing import TYPE_CHECKING
from BaseClasses import Entrance, Region

if TYPE_CHECKING:
    from .world import GTAIVWorld

def create_and_connect_regions(world: GTAIVWorld) -> None:
    create_all_regions(world)
    connect_regions(world)

def create_all_regions(world: GTAIVWorld) -> None:
    # Create the TBoGT Islands
    # 'Algonquin' is the typical TBoGT starting island
    algonquin = Region("Algonquin", world.player, world.multiworld)
    broker = Region("Broker", world.player, world.multiworld)
    alderney = Region("Alderney", world.player, world.multiworld)

    world.multiworld.regions += [algonquin, broker, alderney]

def connect_regions(world: GTAIVWorld) -> None:
    algonquin = world.get_region("Algonquin")
    broker = world.get_region("Broker")
    alderney = world.get_region("Alderney")

    # Connect islands via bridges (Names must match world.get_entrance in rules.py)
    algonquin.connect(broker, "Bridge to Broker")
    algonquin.connect(alderney, "Bridge to Alderney")