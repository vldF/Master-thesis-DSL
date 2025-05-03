from collections.abc import Mapping
from typing import Any, TypeVar, Union

from attrs import define as _attrs_define
from attrs import field as _attrs_field

from ..types import UNSET, Unset

T = TypeVar("T", bound="InlineResponse200Timezone")


@_attrs_define
class InlineResponse200Timezone:
    """
    Attributes:
        abbreviation (Union[Unset, str]):
        current_time (Union[Unset, str]):
        gmt_offset (Union[Unset, int]):
        is_dst (Union[Unset, bool]):
        name (Union[Unset, str]):
    """

    abbreviation: Union[Unset, str] = UNSET
    current_time: Union[Unset, str] = UNSET
    gmt_offset: Union[Unset, int] = UNSET
    is_dst: Union[Unset, bool] = UNSET
    name: Union[Unset, str] = UNSET
    additional_properties: dict[str, Any] = _attrs_field(init=False, factory=dict)

    def to_dict(self) -> dict[str, Any]:
        abbreviation = self.abbreviation

        current_time = self.current_time

        gmt_offset = self.gmt_offset

        is_dst = self.is_dst

        name = self.name

        field_dict: dict[str, Any] = {}
        field_dict.update(self.additional_properties)
        field_dict.update({})
        if abbreviation is not UNSET:
            field_dict["abbreviation"] = abbreviation
        if current_time is not UNSET:
            field_dict["current_time"] = current_time
        if gmt_offset is not UNSET:
            field_dict["gmt_offset"] = gmt_offset
        if is_dst is not UNSET:
            field_dict["is_dst"] = is_dst
        if name is not UNSET:
            field_dict["name"] = name

        return field_dict

    @classmethod
    def from_dict(cls: type[T], src_dict: Mapping[str, Any]) -> T:
        d = dict(src_dict)
        abbreviation = d.pop("abbreviation", UNSET)

        current_time = d.pop("current_time", UNSET)

        gmt_offset = d.pop("gmt_offset", UNSET)

        is_dst = d.pop("is_dst", UNSET)

        name = d.pop("name", UNSET)

        inline_response_200_timezone = cls(
            abbreviation=abbreviation,
            current_time=current_time,
            gmt_offset=gmt_offset,
            is_dst=is_dst,
            name=name,
        )

        inline_response_200_timezone.additional_properties = d
        return inline_response_200_timezone

    @property
    def additional_keys(self) -> list[str]:
        return list(self.additional_properties.keys())

    def __getitem__(self, key: str) -> Any:
        return self.additional_properties[key]

    def __setitem__(self, key: str, value: Any) -> None:
        self.additional_properties[key] = value

    def __delitem__(self, key: str) -> None:
        del self.additional_properties[key]

    def __contains__(self, key: str) -> bool:
        return key in self.additional_properties
