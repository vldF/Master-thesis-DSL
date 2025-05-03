from collections.abc import Mapping
from typing import Any, TypeVar, Union

from attrs import define as _attrs_define
from attrs import field as _attrs_field

from ..types import UNSET, Unset

T = TypeVar("T", bound="InlineResponse200Connection")


@_attrs_define
class InlineResponse200Connection:
    """
    Attributes:
        autonomous_system_number (Union[Unset, int]):
        autonomous_system_organization (Union[Unset, str]):
        connection_type (Union[Unset, str]):
        isp_name (Union[Unset, str]):
        organization_name (Union[Unset, str]):
    """

    autonomous_system_number: Union[Unset, int] = UNSET
    autonomous_system_organization: Union[Unset, str] = UNSET
    connection_type: Union[Unset, str] = UNSET
    isp_name: Union[Unset, str] = UNSET
    organization_name: Union[Unset, str] = UNSET
    additional_properties: dict[str, Any] = _attrs_field(init=False, factory=dict)

    def to_dict(self) -> dict[str, Any]:
        autonomous_system_number = self.autonomous_system_number

        autonomous_system_organization = self.autonomous_system_organization

        connection_type = self.connection_type

        isp_name = self.isp_name

        organization_name = self.organization_name

        field_dict: dict[str, Any] = {}
        field_dict.update(self.additional_properties)
        field_dict.update({})
        if autonomous_system_number is not UNSET:
            field_dict["autonomous_system_number"] = autonomous_system_number
        if autonomous_system_organization is not UNSET:
            field_dict["autonomous_system_organization"] = autonomous_system_organization
        if connection_type is not UNSET:
            field_dict["connection_type"] = connection_type
        if isp_name is not UNSET:
            field_dict["isp_name"] = isp_name
        if organization_name is not UNSET:
            field_dict["organization_name"] = organization_name

        return field_dict

    @classmethod
    def from_dict(cls: type[T], src_dict: Mapping[str, Any]) -> T:
        d = dict(src_dict)
        autonomous_system_number = d.pop("autonomous_system_number", UNSET)

        autonomous_system_organization = d.pop("autonomous_system_organization", UNSET)

        connection_type = d.pop("connection_type", UNSET)

        isp_name = d.pop("isp_name", UNSET)

        organization_name = d.pop("organization_name", UNSET)

        inline_response_200_connection = cls(
            autonomous_system_number=autonomous_system_number,
            autonomous_system_organization=autonomous_system_organization,
            connection_type=connection_type,
            isp_name=isp_name,
            organization_name=organization_name,
        )

        inline_response_200_connection.additional_properties = d
        return inline_response_200_connection

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
