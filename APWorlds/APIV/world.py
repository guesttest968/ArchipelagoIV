from collections.abc import Mapping
from typing import Any

from worlds.AutoWorld import World

from . import items, locations, regions, rules


class GTAIVWorld(World):
    """
    Grand Theft Auto IV is a gritty, realistic open-world action-adventure game set in a parody of New York City, Liberty City. 
    Players control Niko Bellic, an Eastern European veteran navigating a dark underworld of crime and corruption while seeking the "American Dream".
    """

    game = "Grand Theft Auto IV"


    location_name_to_id = locations.LOCATION_NAME_TO_ID
    item_name_to_id = items.ITEM_NAME_TO_ID


    origin_region_name = "Broker"


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


