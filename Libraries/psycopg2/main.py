from flask import Flask, request
from psycopg2 import connect

app = Flask()

# no vulnerability
@app.route("no_vulner/<user_id>/")
def no_vulner():
    user_id = request.args.get('user_id')
    conn = connect("dbname=test user=postgres")
    cur = conn.cursor()
    cur.execute("SELECT * FROM table WHERE ID = %s", (user_id, ))
    return ""

# vulnerability
@app.route("vulner/")
def vulner():
    user_id = request.args.get('user_id')
    conn = connect("dbname=test user=postgres")
    cur = conn.cursor()
    cur.execute("SELECT * FROM table WHERE ID = " + user_id, ())
    return ""

# vulnerability
@app.route("vulner2/")
def vulner():
    user_id = request.args.get('user_id')
    conn = connect("dbname=test user=postgres")
    cur = conn.cursor()
    cur.execute("SELECT * FROM table WHERE ID = " + user_id, ())
    return cur.fetchone()
