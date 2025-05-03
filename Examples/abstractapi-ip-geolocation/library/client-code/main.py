from flask import Flask, escape
from ipDeolocationProvider import IpGeolocationClient

app = Flask(__name__)

ip_client = IpGeolocationClient(base_url="https://ipgeolocation.abstractapi.com/", token="api-token")
def get_ip_info(ip: str) -> str:
    return ip_client.get_info(ip)

@app.route('/vulnerable/ip_info/<int:user_ip>', methods=['GET'])
def get_user_session(user_ip):
    ip_info = get_ip_info(user_ip)
    escaped_ip = escape(user_ip)
    # possible XSS vulnerability if IP Geolocation provider send vulnerable data
    return f"""
    <html>
    <b>IP: {escaped_ip}</b>
    </br>
    <b>IP info: {ip_info}</b>
    </html>
    """

@app.route('/safe/ip_info/<int:user_ip>', methods=['GET'])
def get_user_session(user_ip):
    ip_info = get_ip_info(user_ip)
    escaped_ip_info = escape(ip_info)
    escaped_ip = escape(user_ip)
    return f"""
    <html>
    <b>IP: {escaped_ip}</b>
    </br>
    <b>IP info: {escaped_ip_info}</b>
    </html>
    """

if __name__ == '__main__':
    app.run(debug=True)
