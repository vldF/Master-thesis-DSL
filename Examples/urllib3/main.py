import os
from flask import Flask, request
import urllib3

app = Flask(__name__)

@app.route('/get-14', methods=['GET'])
def get_function_14():
    url = request.args.get('url', "https://www.ptsecurity.com")
    try:
        sensitive_data = os.environ['USERNAME']
        urllib3.request("GET", "url", b"", headers={"sens": sensitive_data })  # Server-Side Request Forgery | Sensitive Information Leakage (via headers)
    except Exception as e:
        print("Error: ", e)
    return 'sent'

if __name__ == '__main__':
    app.run(debug=True, port=5000, host='0.0.0.0')
