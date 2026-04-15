from __future__ import annotations

from typing import TYPE_CHECKING, Dict

from BaseClasses import Item, ItemClassification

if TYPE_CHECKING:
    from .world import GTAIVWorld



GTAIV_WEAPON_ID = {
    "Assault Rifle":    800001,
    "Baseball Bat":     800002,
    "Carbine Rifle":    800003,
    "Combat Pistol":    800004,
    "Combat Shotgun":   800005,
    "Combat Sniper":    800006,
    "Grenade":          800007,
    "Knife":            800008,
    "Micro SMG":        800009,
    "Molotov Cocktail": 800010,
    "Pistol":           800011,
    "Pump Shotgun":     800012,
    "RPG":              800013,
    "SMG":              800014,
    "Sniper Rifle":     800015,
    "Body Armor":       800016,
}


GTAIV_TBOGT_WEAPON_ID = {
    "Pistol .44":               800023,
    "AA12 Shotgun":             800031,
    "Explosive Shotgun":        800024,
    "Gold SMG":                 800025,
    "Assault SMG":              800026,
    "Advanced MG":              800027,
    "Advanced Sniper":          800028,
    "Sticky Bomb":              800029,
    "Parachute":                800030
}

RAGE_GENERIC_FILLER_ID = {
    "Heal Player":           800100, # Generic Heals
    "Money $500":       800101, 
    "Wanted Level 1":   800102,
    "Wanted Level 2":   800103,
    "Wanted Level 3":   800104,
    "Wanted Level 4":   800105,
    "Wanted Level 5":   800106,
    "Wanted Level 6":   800107,
    "Clear Wanted Level":   800108,
    "Ammo":       800109,
    "Drunk Camera 10 Seconds": 800110,
    "Forver Wanted": 800111
}

GTAIV_CORE_ISLAND_UNLOCKS_ID = {
    "Corner Kids":  800061, 
    "This Ain't Checkers":800061, 
    
    "Departure Time": 800062,

}


GTAIV_MISC_CLASSIFICATIONS = {

    "Heal Player":          ItemClassification.useful, 
    "Money $500":           ItemClassification.useful,
    "Wanted Level 1":       ItemClassification.trap,
    "Wanted Level 2":       ItemClassification.trap,
    "Wanted Level 3":       ItemClassification.trap,
    "Wanted Level 4":       ItemClassification.trap,
    "Wanted Level 5":       ItemClassification.trap,
    "Wanted Level 6":       ItemClassification.trap,
    "Clear Wanted Level":       ItemClassification.trap,
    "Ammo":       ItemClassification.useful,
    "Drunk Camera 10 Seconds": ItemClassification.trap,
    "Forver Wanted": ItemClassification.trap

}

GTAIV_WEAPON_CLASSIFICATIONS = {
    "Assault Rifle":        ItemClassification.useful,
    "Carbine Rifle":        ItemClassification.useful,
    "RPG":                  ItemClassification.useful,
    "Sniper Rifle":         ItemClassification.useful,
    "Combat Sniper":        ItemClassification.useful,

    "Combat Shotgun":       ItemClassification.useful,
    "Micro SMG":            ItemClassification.useful,
    "SMG":                  ItemClassification.useful,
    "Body Armor":           ItemClassification.useful,
    "Grenade":              ItemClassification.useful,
    "Molotov Cocktail":     ItemClassification.useful,
    "Pump Shotgun":         ItemClassification.useful,

    "Baseball Bat":         ItemClassification.filler,
    "Knife":                ItemClassification.filler,
    "Pistol":               ItemClassification.filler,
    "Combat Pistol":        ItemClassification.filler,
}


GTAIV_TBOGT_WEAPON_CLASSIFICATIONS = {
    "Pistol .44":               ItemClassification.filler,
    "Explosive shotgun":        ItemClassification.useful,
    "Gold SMG":                 ItemClassification.useful,
    "Assault SMG":              ItemClassification.useful,
    "Advanced MG":              ItemClassification.useful,
    "Advanced sniper":          ItemClassification.useful,
    "Sticky bomb":              ItemClassification.useful,
    "Parachute":                ItemClassification.filler
}

GTAIV_CORE_MISSIONS_CLASSIFICATIONS = {
    "Corner Kids":      ItemClassification.progression,
    "This Ain't Checkers":    ItemClassification.progression,
    "Departure Time":    ItemClassification.progression,
}

ITEM_NAME_TO_ID = GTAIV_WEAPON_ID | RAGE_GENERIC_FILLER_ID | GTAIV_TBOGT_WEAPON_ID | GTAIV_CORE_ISLAND_UNLOCKS_ID


class GTAIVItem(Item):
    game = "Grand Theft Auto IV: The Lost and Damned"

def get_random_filler_item_name(world: GTAIVWorld) -> str:
    filler_options = list(GTAIV_MISC_CLASSIFICATIONS.keys())
    return world.random.choice(filler_options)
        
def create_item_with_correct_classification(world: GTAIVWorld, name: str) -> GTAIVItem:
    master_class_dict = GTAIV_MISC_CLASSIFICATIONS | GTAIV_WEAPON_CLASSIFICATIONS | GTAIV_TBOGT_WEAPON_CLASSIFICATIONS
    classification = master_class_dict[name]
    return GTAIVItem(name, classification, ITEM_NAME_TO_ID[name], world.player)

def create_all_items(world: GTAIVWorld) -> None:

    core_items = list(GTAIV_WEAPON_CLASSIFICATIONS.keys() | GTAIV_TBOGT_WEAPON_CLASSIFICATIONS.keys())

    itempool: list[Item] = []
    for name in core_items:
        itempool.append(create_item_with_correct_classification(world, name))


    number_of_items = len(itempool)
    number_of_unfilled_locations = len(world.multiworld.get_unfilled_locations(world.player))
    needed_filler = number_of_unfilled_locations - number_of_items

    for _ in range(needed_filler):
        itempool.append(world.create_filler())


    world.multiworld.itempool += itempool


    # if world.options.start_with_pistol:
    #    world.push_precollected(create_item_with_correct_classification(world, "Pistol"))