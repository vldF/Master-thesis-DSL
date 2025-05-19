from flask import Flask, request, redirect, jsonify

from psycopg2 import connect

app = Flask(__name__)


@app.route("/auth/<string:redirect_url>")
def auth(redirect_url: str):
  login = request.args["user_login"]
  pass_hash = request.args["user_login"]
  if not validate_login_and_password(login, pass_hash):
    return "Login failed", 401

  token = get_user_token(login)
  # уязвимость: подделка запросов со стороны сервера
  return redirect(f"{redirect_url}?token={token}")

def validate_login_and_password(login: str, pass_hash: str) -> bool:
  conn = connect("dbname=test user=postgres")
  cur = conn.cursor()
  cur.execute("SELECT * FROM users WHERE login = %s", (login, ))
  user = cur.fetchone()
  return user is not None and user.pass_hash == pass_hash

@app.route("/user/get_me/<string:token>")
def get_current_user(token: str):
  current_user = get_user_by_token(token)
  if current_user is None:
    # уязвимость: межсайтовый скриптинг
    return f"invalid token: {token}"

  return jsonify(current_user)

def get_user_by_token(token: str):
  conn = connect("dbname=test user=postgres")
  cur = conn.cursor()

  cur.execute("SELECT * FROM user_tokens WHERE token = %s", (token, ))
  token_record = cur.fetchone()
  if token_record is None:
    return None

  cur.execute("SELECT * FROM users WHERE login = %s", (token_record.login, ))
  user = cur.fetchone()

  return user

def get_user_token(login) -> str:
  pass
