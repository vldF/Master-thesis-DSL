from http import HTTPStatus
from typing import Any, Optional, Union

import httpx

from ... import errors
from ...client import AuthenticatedClient, Client
from ...models.inline_response_200 import InlineResponse200
from ...types import UNSET, Response, Unset


def _get_kwargs(
    *,
    api_key: str,
    ip_address: Union[Unset, str] = UNSET,
    fields: Union[Unset, str] = UNSET,
) -> dict[str, Any]:
    params: dict[str, Any] = {}

    params["api_key"] = api_key

    params["ip_address"] = ip_address

    params["fields"] = fields

    params = {k: v for k, v in params.items() if v is not UNSET and v is not None}

    _kwargs: dict[str, Any] = {
        "method": "get",
        "url": "/v1/",
        "params": params,
    }

    return _kwargs


def _parse_response(
    *, client: Union[AuthenticatedClient, Client], response: httpx.Response
) -> Optional[InlineResponse200]:
    if response.status_code == 200:
        response_200 = InlineResponse200.from_dict(response.json())

        return response_200
    if client.raise_on_unexpected_status:
        raise errors.UnexpectedStatus(response.status_code, response.content)
    else:
        return None


def _build_response(
    *, client: Union[AuthenticatedClient, Client], response: httpx.Response
) -> Response[InlineResponse200]:
    return Response(
        status_code=HTTPStatus(response.status_code),
        content=response.content,
        headers=response.headers,
        parsed=_parse_response(client=client, response=response),
    )


def sync_detailed(
    *,
    client: Union[AuthenticatedClient, Client],
    api_key: str,
    ip_address: Union[Unset, str] = UNSET,
    fields: Union[Unset, str] = UNSET,
) -> Response[InlineResponse200]:
    """Retrieve the location of an IP address

    Args:
        api_key (str):
        ip_address (Union[Unset, str]):  Example: 195.154.25.40.
        fields (Union[Unset, str]):  Example: country,city,timezone.

    Raises:
        errors.UnexpectedStatus: If the server returns an undocumented status code and Client.raise_on_unexpected_status is True.
        httpx.TimeoutException: If the request takes longer than Client.timeout.

    Returns:
        Response[InlineResponse200]
    """

    kwargs = _get_kwargs(
        api_key=api_key,
        ip_address=ip_address,
        fields=fields,
    )

    response = client.get_httpx_client().request(
        **kwargs,
    )

    return _build_response(client=client, response=response)


def sync(
    *,
    client: Union[AuthenticatedClient, Client],
    api_key: str,
    ip_address: Union[Unset, str] = UNSET,
    fields: Union[Unset, str] = UNSET,
) -> Optional[InlineResponse200]:
    """Retrieve the location of an IP address

    Args:
        api_key (str):
        ip_address (Union[Unset, str]):  Example: 195.154.25.40.
        fields (Union[Unset, str]):  Example: country,city,timezone.

    Raises:
        errors.UnexpectedStatus: If the server returns an undocumented status code and Client.raise_on_unexpected_status is True.
        httpx.TimeoutException: If the request takes longer than Client.timeout.

    Returns:
        InlineResponse200
    """

    return sync_detailed(
        client=client,
        api_key=api_key,
        ip_address=ip_address,
        fields=fields,
    ).parsed


async def asyncio_detailed(
    *,
    client: Union[AuthenticatedClient, Client],
    api_key: str,
    ip_address: Union[Unset, str] = UNSET,
    fields: Union[Unset, str] = UNSET,
) -> Response[InlineResponse200]:
    """Retrieve the location of an IP address

    Args:
        api_key (str):
        ip_address (Union[Unset, str]):  Example: 195.154.25.40.
        fields (Union[Unset, str]):  Example: country,city,timezone.

    Raises:
        errors.UnexpectedStatus: If the server returns an undocumented status code and Client.raise_on_unexpected_status is True.
        httpx.TimeoutException: If the request takes longer than Client.timeout.

    Returns:
        Response[InlineResponse200]
    """

    kwargs = _get_kwargs(
        api_key=api_key,
        ip_address=ip_address,
        fields=fields,
    )

    response = await client.get_async_httpx_client().request(**kwargs)

    return _build_response(client=client, response=response)


async def asyncio(
    *,
    client: Union[AuthenticatedClient, Client],
    api_key: str,
    ip_address: Union[Unset, str] = UNSET,
    fields: Union[Unset, str] = UNSET,
) -> Optional[InlineResponse200]:
    """Retrieve the location of an IP address

    Args:
        api_key (str):
        ip_address (Union[Unset, str]):  Example: 195.154.25.40.
        fields (Union[Unset, str]):  Example: country,city,timezone.

    Raises:
        errors.UnexpectedStatus: If the server returns an undocumented status code and Client.raise_on_unexpected_status is True.
        httpx.TimeoutException: If the request takes longer than Client.timeout.

    Returns:
        InlineResponse200
    """

    return (
        await asyncio_detailed(
            client=client,
            api_key=api_key,
            ip_address=ip_address,
            fields=fields,
        )
    ).parsed
