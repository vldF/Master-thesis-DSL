from flask import Flask, request, jsonify

app = Flask(__name__)

@app.route("/<string:test>")
def hello_world(test):
    return test

# no vulnerability
@app.route("/test1")
def test_req1():
    return request.blueprint

# vulnerability
@app.route("/test2")
def test_req2():
    return request.args["arg"]

# vulnerability
@app.route("/vulner/<string:test>")
def test_req3(test):
    return jsonify(test)
