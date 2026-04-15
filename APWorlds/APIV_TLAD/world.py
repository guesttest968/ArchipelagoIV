from collections.abc import Mapping
from typing import Any

from worlds.AutoWorld import World

from . import items, locations, regions, rules



class GTAIVWorld(World):
    """
    Grand Theft Auto IV: The Lost and Damned is the first of two episodes for Grand Theft Auto IV. 
    It features Johnny Klebitz, the Vice President of The Lost Motorcycle Club, who struggles to keep the gang together amidst a brewing war and the return of their volatile leader, Billy Grey.
    The storyline intertwines with the original game and first episode, most notably the Museum Piece mission that involves all three protagonists.
    """

    game = "Grand Theft Auto IV: The Lost and Damned"

    location_name_to_id = locations.LOCATION_NAME_TO_ID_TLAD
    item_name_to_id = items.ITEM_NAME_TO_ID 

    origin_region_name = "Algonquin"


    def create_regions(self) -> None:
        regions.create_and_connect_regions(self)
        locations.create_all_locations(self)

    def set_rules(self) -> None:
        rules.set_all_rules(self)

    def create_items(self) -> None:
            items.create_all_items(self)

    def create_item(self, name: str) -> items.GTAIVItem:
        return items.create_item_with_correct_classification(self, name)



    def get_filler_item_name(self) -> str:
        return items.get_random_filler_item_name(self)
        


