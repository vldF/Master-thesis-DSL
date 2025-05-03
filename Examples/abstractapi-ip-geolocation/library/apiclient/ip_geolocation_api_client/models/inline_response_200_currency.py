from collections.abc import Mapping
from typing import Any, TypeVar, Union

from attrs import define as _attrs_define
from attrs import field as _attrs_field

from ..types import UNSET, Unset

T = TypeVar("T", bound="InlineResponse200Currency")


@_attrs_define
class InlineResponse200Currency:
    """
    Attributes:
        currency_code (Union[Unset, str]):
        currency_name (Union[Unset, str]):
    """

    currency_code: Union[Unset, str] = UNSET
    currency_name: Union[Unset, str] = UNSET
    additional_properties: dict[str, Any] = _attrs_field(init=False, factory=dict)

    def to_dict(self) -> dict[str, Any]:
        currency_code = self.currency_code

        currency_name = self.currency_name

        field_dict: dict[str, Any] = {}
        field_dict.update(self.additional_properties)
        field_dict.update({})
        if currency_code is not UNSET:
            field_dict["currency_code"] = currency_code
        if currency_name is not UNSET:
            field_dict["currency_name"] = currency_name

        return field_dict

    @classmethod
    def from_dict(cls: type[T], src_dict: Mapping[str, Any]) -> T:
        d = dict(src_dict)
        currency_code = d.pop("currency_code", UNSET)

        currency_name = d.pop("currency_name", UNSET)

        inline_response_200_currency = cls(
            currency_code=currency_code,
            currency_name=currency_name,
        )

        inline_response_200_currency.additional_properties = d
        return inline_response_200_currency

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
