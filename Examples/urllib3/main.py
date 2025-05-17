import os
import tempfile

import urllib3
from flask import Flask, request

app = Flask(__name__)

@app.route('/save_profile_image', methods=['GET'])
def save_profile_image():
    image_url = request.args.get('image_url')
    temp_file = tempfile.TemporaryFile()
    download_image(image_url, temp_file)

    process_image(temp_file)

    return

def download_image(url, filename):
    token = get_system_token()
    response = urllib3.request('GET', url, { 'auth_token': token }) # Server-Side Request Forgery

    with open(filename, 'wb') as f:
        f.write(response.data)

    print(f"Image saved as {filename}")

def process_image(file):
    pass

def get_system_token():
    return ""

if __name__ == '__main__':
    app.run(debug=True, port=5000, host='0.0.0.0')
