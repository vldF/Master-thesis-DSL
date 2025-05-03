import grpc
import userstorage.services_pb2 as user_storage_dto
import userstorage.services_pb2_grpc as user_storage_service
from flask import Flask, jsonify

SERVER_URL = 'http://server'

app = Flask(__name__)

@app.route("/vulnerable/getCurrentUserInfo/<string:user_id>")
def vulnerable_get_user_info(user_id: str):
    with grpc.insecure_channel(SERVER_URL) as channel:
        user_storage_stub = user_storage_service.UserStoreStub(channel)
        user = user_storage_stub.GetUser(user_storage_dto.UserRequest(user_id))  # vulnerability: insecure direct object reference

        return jsonify(user)

def _get_user_id() -> str:
    pass

@app.route("/safe/getUserInfo/")
def safe_get_user_info():
    current_user_id = _get_user_id()
    with grpc.insecure_channel(SERVER_URL) as channel:
        user_storage_stub = user_storage_service.UserStoreStub(channel)
        user = user_storage_stub.GetUser(user_storage_dto.UserRequest(current_user_id))  # no vulnerability

        return jsonify(user)
