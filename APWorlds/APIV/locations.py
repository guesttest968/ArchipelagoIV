from __future__ import annotations
from typing import TYPE_CHECKING, Dict
from BaseClasses import Location

if TYPE_CHECKING:
    from .world import GTAIVWorld

LOCATION_NAME_TO_ID: Dict[str, int] = {
    "The Cousins Bellic": 810001,
    "Easy Fare": 810002,
    "Uncle Vlad": 810003,
    "Crime and Punishment": 810004,
    "The Master and the Molotov": 810005,
    "Russian Revolution": 810006,
    "Roman's Sorrow": 810007,
    "Luck of the Irish": 810008,
    "Blow Your Cover": 810009,
    "Three Leaf Clover": 810010,
    "The Holland Play": 810011,
    "Blood Brothers": 810012,
    "Museum Piece": 810013,
    "Weekend at Florian's": 810014,
    "That Special Someone": 810015,
    "One Last Thing": 810016,
    "If the Price Is Right": 810017,
    "Mr. and Mrs. Bellic (Deal)": 810018,
    "A Revenger's Tragedy": 810019,
    "A Dish Served Cold": 810020,
    "Mr. and Mrs. Bellic (Revenge)": 810021,
    "Out of Commission": 810022,
}

class GTAIVLocation(Location):
    game = "Grand Theft Auto IV"

def create_all_locations(world: GTAIVWorld) -> None:
    broker_region = world.get_region("Broker")
    
    for name, id in LOCATION_NAME_TO_ID.items():
        location = GTAIVLocation(world.player, name, id, broker_region)
        broker_region.locations.append(location)