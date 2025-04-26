from flask import Flask, request

app = Flask()

@app.route("/<string:test>")
def hello_world(test):
    return test

# no vulnerability
@app.route("/test1")
def test_req():
    return request.blueprint

# vulnerability
@app.route("/test2")
def test_req():
    return request.args["arg"]
