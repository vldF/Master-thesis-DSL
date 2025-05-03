# /bin/bash

python3.11 -m grpc_tools.protoc -I./protos --python_out=./userstorage --pyi_out=./userstorage --grpc_python_out=./userstorage ./protos/services.proto
