from __future__ import annotations
from typing import TYPE_CHECKING, Dict
from BaseClasses import Location

if TYPE_CHECKING:
    from .world import GTAIVWorld

# Full mapping of GTA IV: The Ballad of Gay Tony Missions to IDs
LOCATION_NAME_TO_ID_TLAD: Dict[str, int] = {
    "I Luv LC": 820001,
    "Chinese Takeout": 820002,
    "Momma's Boy": 820003,
    "Corner Kids": 820004,
    "Clocking Off": 820005,
    "Practice Swing": 820006,
    "Blog This!...": 820007,
    "Bang Bang": 820008,
    "Boulevard Baby": 820009,
    "Frosting on the Cake": 820010,
    "Kibbutz Number One": 820011,
    "Sexy Time": 820012,
    "High Dive": 820013,
    "...Blog This!": 820014,
    "This Ain't Checkers": 820015,
    "Not So Fast": 820016,
    "Ladies' Night": 820017,
    "No. 3": 820018,
    "Going Deep": 820019,
    "Caught with your Pants Down": 820020,
    "Dropping In": 820021,
    "In the Crosshairs": 820022,
    "For the Man Who Has Everything": 820023,
    "Ladies Half Price": 820024,
    "Party's Over": 820025,
    "Departure Time": 820026,
}

class GTAIVLocation(Location):
    game = "Grand Theft Auto IV: The Lost and Damned"

def create_all_locations(world: GTAIVWorld) -> None:
    broker_region = world.get_region("Broker")
    algonquin_region = world.get_region("Algonquin")
    
    # You will need to separate your locations based on where they actually are
    for name, id in LOCATION_NAME_TO_ID_TLAD.items():
        # Example fix: Put "Corner Kids" in Algonquin so the player can actually reach it
        if name in ["Corner Kids", "I Luv LC", "Chinese Takeout", "Momma's Boy"]:
            location = GTAIVLocation(world.player, name, id, algonquin_region)
            algonquin_region.locations.append(location)
        else:
            location = GTAIVLocation(world.player, name, id, broker_region)
            broker_region.locations.append(location)