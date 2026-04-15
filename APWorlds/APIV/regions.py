from __future__ import annotations
from typing import TYPE_CHECKING
from BaseClasses import Entrance, Region

if TYPE_CHECKING:
    from .world import GTAIVWorld

def create_and_connect_regions(world: GTAIVWorld) -> None:
    create_all_regions(world)
    connect_regions(world)

def create_all_regions(world: GTAIVWorld) -> None:
    # Broker/Dukes is our starting area (Origin)
    broker = Region("Broker", world.player, world.multiworld)
    bohan = Region("Bohan", world.player, world.multiworld)
    algonquin = Region("Algonquin", world.player, world.multiworld)
    alderney = Region("Alderney", world.player, world.multiworld)

    regions = [broker, bohan, algonquin, alderney]
    world.multiworld.regions += regions

def connect_regions(world: GTAIVWorld) -> None:
    broker = world.get_region("Broker")
    bohan = world.get_region("Bohan")
    algonquin = world.get_region("Algonquin")
    alderney = world.get_region("Alderney")

    broker.connect(bohan, "Broker to Bohan")

    broker.connect(algonquin, "Bridge to Algonquin", 
                   lambda state: state.has("Blow Your Cover", world.player))

    algonquin.connect(alderney, "Bridge to Alderney", 
                      lambda state: state.has("Three Leaf Clover", world.player))
