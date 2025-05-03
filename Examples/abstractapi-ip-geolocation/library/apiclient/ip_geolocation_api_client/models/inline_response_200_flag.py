from collections.abc import Mapping
from typing import Any, TypeVar, Union

from attrs import define as _attrs_define
from attrs import field as _attrs_field

from ..types import UNSET, Unset

T = TypeVar("T", bound="InlineResponse200Flag")


@_attrs_define
class InlineResponse200Flag:
    """
    Attributes:
        emoji (Union[Unset, str]):
        png (Union[Unset, str]):
        svg (Union[Unset, str]):
        unicode (Union[Unset, str]):
    """

    emoji: Union[Unset, str] = UNSET
    png: Union[Unset, str] = UNSET
    svg: Union[Unset, str] = UNSET
    unicode: Union[Unset, str] = UNSET
    additional_properties: dict[str, Any] = _attrs_field(init=False, factory=dict)

    def to_dict(self) -> dict[str, Any]:
        emoji = self.emoji

        png = self.png

        svg = self.svg

        unicode = self.unicode

        field_dict: dict[str, Any] = {}
        field_dict.update(self.additional_properties)
        field_dict.update({})
        if emoji is not UNSET:
            field_dict["emoji"] = emoji
        if png is not UNSET:
            field_dict["png"] = png
        if svg is not UNSET:
            field_dict["svg"] = svg
        if unicode is not UNSET:
            field_dict["unicode"] = unicode

        return field_dict

    @classmethod
    def from_dict(cls: type[T], src_dict: Mapping[str, Any]) -> T:
        d = dict(src_dict)
        emoji = d.pop("emoji", UNSET)

        png = d.pop("png", UNSET)

        svg = d.pop("svg", UNSET)

        unicode = d.pop("unicode", UNSET)

        inline_response_200_flag = cls(
            emoji=emoji,
            png=png,
            svg=svg,
            unicode=unicode,
        )

        inline_response_200_flag.additional_properties = d
        return inline_response_200_flag

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
