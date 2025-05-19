from flask import Flask, request

from psycopg2 import connect

app = Flask()

@app.route("users/description/<user_id>/", methods=["GET"])
def get_user_description():
  user_id = request.args.get('user_id')
  conn = connect("dbname=test user=postgres")
  cur = conn.cursor()
  # уязвимость: внедрение SQL-кода
  cur.execute("SELECT description FROM table WHERE ID = " + user_id)
  description = cur.fetchone()
  # уязвимость второго порядка: межсайтовый скриптинг
  return description

@app.route("users/<user_id>/", methods=["GET"])
def get_user1():
  user_id = request.args.get('user_id')
  conn = connect("dbname=test user=postgres")
  cur = conn.cursor()
  # уязвимость: внедрение SQL-кода
  cur.execute("SELECT * FROM table WHERE ID = " + user_id)
  user = cur.fetchone()
  # нет уязвимости
  return jsonify(user)

@app.route("users/<user_id>/", methods=["GET"])
def get_user2():
  user_id = request.args.get('user_id')
  conn = connect("dbname=test user=postgres")
  cur = conn.cursor()
  # нет уязвимости
  cur.execute("SELECT * FROM table WHERE ID = %s", (user_id, ))
  user = cur.fetchone()
  # нет уязвимости
  return jsonify(user)
