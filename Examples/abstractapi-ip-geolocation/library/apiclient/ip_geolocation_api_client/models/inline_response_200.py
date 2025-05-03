from collections.abc import Mapping
from typing import TYPE_CHECKING, Any, TypeVar, Union

from attrs import define as _attrs_define
from attrs import field as _attrs_field

from ..types import UNSET, Unset

if TYPE_CHECKING:
    from ..models.inline_response_200_connection import InlineResponse200Connection
    from ..models.inline_response_200_currency import InlineResponse200Currency
    from ..models.inline_response_200_flag import InlineResponse200Flag
    from ..models.inline_response_200_security import InlineResponse200Security
    from ..models.inline_response_200_timezone import InlineResponse200Timezone


T = TypeVar("T", bound="InlineResponse200")


@_attrs_define
class InlineResponse200:
    """
    Attributes:
        city (Union[Unset, str]):
        city_geoname_id (Union[Unset, int]):
        connection (Union[Unset, InlineResponse200Connection]):
        continent (Union[Unset, str]):
        continent_code (Union[Unset, str]):
        continent_geoname_id (Union[Unset, int]):
        country (Union[Unset, str]):
        country_code (Union[Unset, str]):
        country_geoname_id (Union[Unset, int]):
        country_is_eu (Union[Unset, bool]):
        currency (Union[Unset, InlineResponse200Currency]):
        flag (Union[Unset, InlineResponse200Flag]):
        ip_address (Union[Unset, str]):
        latitude (Union[Unset, float]):
        longitude (Union[Unset, float]):
        postal_code (Union[Unset, str]):
        region (Union[Unset, str]):
        region_geoname_id (Union[Unset, int]):
        region_iso_code (Union[Unset, str]):
        security (Union[Unset, InlineResponse200Security]):
        timezone (Union[Unset, InlineResponse200Timezone]):
    """

    city: Union[Unset, str] = UNSET
    city_geoname_id: Union[Unset, int] = UNSET
    connection: Union[Unset, "InlineResponse200Connection"] = UNSET
    continent: Union[Unset, str] = UNSET
    continent_code: Union[Unset, str] = UNSET
    continent_geoname_id: Union[Unset, int] = UNSET
    country: Union[Unset, str] = UNSET
    country_code: Union[Unset, str] = UNSET
    country_geoname_id: Union[Unset, int] = UNSET
    country_is_eu: Union[Unset, bool] = UNSET
    currency: Union[Unset, "InlineResponse200Currency"] = UNSET
    flag: Union[Unset, "InlineResponse200Flag"] = UNSET
    ip_address: Union[Unset, str] = UNSET
    latitude: Union[Unset, float] = UNSET
    longitude: Union[Unset, float] = UNSET
    postal_code: Union[Unset, str] = UNSET
    region: Union[Unset, str] = UNSET
    region_geoname_id: Union[Unset, int] = UNSET
    region_iso_code: Union[Unset, str] = UNSET
    security: Union[Unset, "InlineResponse200Security"] = UNSET
    timezone: Union[Unset, "InlineResponse200Timezone"] = UNSET
    additional_properties: dict[str, Any] = _attrs_field(init=False, factory=dict)

    def to_dict(self) -> dict[str, Any]:
        city = self.city

        city_geoname_id = self.city_geoname_id

        connection: Union[Unset, dict[str, Any]] = UNSET
        if not isinstance(self.connection, Unset):
            connection = self.connection.to_dict()

        continent = self.continent

        continent_code = self.continent_code

        continent_geoname_id = self.continent_geoname_id

        country = self.country

        country_code = self.country_code

        country_geoname_id = self.country_geoname_id

        country_is_eu = self.country_is_eu

        currency: Union[Unset, dict[str, Any]] = UNSET
        if not isinstance(self.currency, Unset):
            currency = self.currency.to_dict()

        flag: Union[Unset, dict[str, Any]] = UNSET
        if not isinstance(self.flag, Unset):
            flag = self.flag.to_dict()

        ip_address = self.ip_address

        latitude = self.latitude

        longitude = self.longitude

        postal_code = self.postal_code

        region = self.region

        region_geoname_id = self.region_geoname_id

        region_iso_code = self.region_iso_code

        security: Union[Unset, dict[str, Any]] = UNSET
        if not isinstance(self.security, Unset):
            security = self.security.to_dict()

        timezone: Union[Unset, dict[str, Any]] = UNSET
        if not isinstance(self.timezone, Unset):
            timezone = self.timezone.to_dict()

        field_dict: dict[str, Any] = {}
        field_dict.update(self.additional_properties)
        field_dict.update({})
        if city is not UNSET:
            field_dict["city"] = city
        if city_geoname_id is not UNSET:
            field_dict["city_geoname_id"] = city_geoname_id
        if connection is not UNSET:
            field_dict["connection"] = connection
        if continent is not UNSET:
            field_dict["continent"] = continent
        if continent_code is not UNSET:
            field_dict["continent_code"] = continent_code
        if continent_geoname_id is not UNSET:
            field_dict["continent_geoname_id"] = continent_geoname_id
        if country is not UNSET:
            field_dict["country"] = country
        if country_code is not UNSET:
            field_dict["country_code"] = country_code
        if country_geoname_id is not UNSET:
            field_dict["country_geoname_id"] = country_geoname_id
        if country_is_eu is not UNSET:
            field_dict["country_is_eu"] = country_is_eu
        if currency is not UNSET:
            field_dict["currency"] = currency
        if flag is not UNSET:
            field_dict["flag"] = flag
        if ip_address is not UNSET:
            field_dict["ip_address"] = ip_address
        if latitude is not UNSET:
            field_dict["latitude"] = latitude
        if longitude is not UNSET:
            field_dict["longitude"] = longitude
        if postal_code is not UNSET:
            field_dict["postal_code"] = postal_code
        if region is not UNSET:
            field_dict["region"] = region
        if region_geoname_id is not UNSET:
            field_dict["region_geoname_id"] = region_geoname_id
        if region_iso_code is not UNSET:
            field_dict["region_iso_code"] = region_iso_code
        if security is not UNSET:
            field_dict["security"] = security
        if timezone is not UNSET:
            field_dict["timezone"] = timezone

        return field_dict

    @classmethod
    def from_dict(cls: type[T], src_dict: Mapping[str, Any]) -> T:
        from ..models.inline_response_200_connection import InlineResponse200Connection
        from ..models.inline_response_200_currency import InlineResponse200Currency
        from ..models.inline_response_200_flag import InlineResponse200Flag
        from ..models.inline_response_200_security import InlineResponse200Security
        from ..models.inline_response_200_timezone import InlineResponse200Timezone

        d = dict(src_dict)
        city = d.pop("city", UNSET)

        city_geoname_id = d.pop("city_geoname_id", UNSET)

        _connection = d.pop("connection", UNSET)
        connection: Union[Unset, InlineResponse200Connection]
        if isinstance(_connection, Unset):
            connection = UNSET
        else:
            connection = InlineResponse200Connection.from_dict(_connection)

        continent = d.pop("continent", UNSET)

        continent_code = d.pop("continent_code", UNSET)

        continent_geoname_id = d.pop("continent_geoname_id", UNSET)

        country = d.pop("country", UNSET)

        country_code = d.pop("country_code", UNSET)

        country_geoname_id = d.pop("country_geoname_id", UNSET)

        country_is_eu = d.pop("country_is_eu", UNSET)

        _currency = d.pop("currency", UNSET)
        currency: Union[Unset, InlineResponse200Currency]
        if isinstance(_currency, Unset):
            currency = UNSET
        else:
            currency = InlineResponse200Currency.from_dict(_currency)

        _flag = d.pop("flag", UNSET)
        flag: Union[Unset, InlineResponse200Flag]
        if isinstance(_flag, Unset):
            flag = UNSET
        else:
            flag = InlineResponse200Flag.from_dict(_flag)

        ip_address = d.pop("ip_address", UNSET)

        latitude = d.pop("latitude", UNSET)

        longitude = d.pop("longitude", UNSET)

        postal_code = d.pop("postal_code", UNSET)

        region = d.pop("region", UNSET)

        region_geoname_id = d.pop("region_geoname_id", UNSET)

        region_iso_code = d.pop("region_iso_code", UNSET)

        _security = d.pop("security", UNSET)
        security: Union[Unset, InlineResponse200Security]
        if isinstance(_security, Unset):
            security = UNSET
        else:
            security = InlineResponse200Security.from_dict(_security)

        _timezone = d.pop("timezone", UNSET)
        timezone: Union[Unset, InlineResponse200Timezone]
        if isinstance(_timezone, Unset):
            timezone = UNSET
        else:
            timezone = InlineResponse200Timezone.from_dict(_timezone)

        inline_response_200 = cls(
            city=city,
            city_geoname_id=city_geoname_id,
            connection=connection,
            continent=continent,
            continent_code=continent_code,
            continent_geoname_id=continent_geoname_id,
            country=country,
            country_code=country_code,
            country_geoname_id=country_geoname_id,
            country_is_eu=country_is_eu,
            currency=currency,
            flag=flag,
            ip_address=ip_address,
            latitude=latitude,
            longitude=longitude,
            postal_code=postal_code,
            region=region,
            region_geoname_id=region_geoname_id,
            region_iso_code=region_iso_code,
            security=security,
            timezone=timezone,
        )

        inline_response_200.additional_properties = d
        return inline_response_200

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
