import time
import zmq
import random
import math
import numpy as np
context = zmq.Context()
socket = context.socket(zmq.REQ)
socket.connect("tcp://localhost:12346")
TIMEOUT = 10000
SKIPFRAME_N = 1000
action = "4 4"
states_hist = []
def getPlayerAction(player_y,coin_y,skul_y,player_skul_dist_x,player_coin_dist,player_skul_dist):
	playerAction = ""
	if player_skul_dist_x < 3.0:
		if player_skul_dist < 0.5:
			playerAction = "2"
		elif player_y > skul_y:
			playerAction = "1"
		else:
			playerAction = "0"
	elif min(player_coin_dist,player_skul_dist) < 0.5:
		if player_coin_dist < player_skul_dist:
			playerAction = "3"
		else:
			playerAction = "2"
	else:
		if player_coin_dist < player_skul_dist:
			if player_y > coin_y :
				playerAction = "1"
			else:
				playerAction = "0"
		else:
			if player_y > skul_y:
				playerAction = "1"
			else:
				playerAction = "0"
	return playerAction
	
actions = ["0", "1", "2","3","4"]  # up, down, push ,pull , stay
						#    0        1     2     3       4    5    6     7       8        9    10    11       12     13
def getAction(states):  # [framenum, catr, dogr, cat_y,  c_x, c_y, sc_c_x, sc_c_y, s_x, s_y, sc_s_x, sc_s_y,  dog_y, isEnd]
	catAction = ""
	dogAction = ""
	cur_state = states
	cat_y = cur_state[3]
	cat_x = -8.0
	dog_x = 8.0
	dog_y = cur_state[-2]
	coin_y = cur_state[5]
	skul_x = cur_state[8]
	skul_y = cur_state[9] 
	
	cat_coin_dist = abs(cat_y - coin_y)
	cat_skul_dist = abs(cat_y - skul_y)
	dog_coin_dist = abs(dog_y - coin_y)
	dog_skul_dist = abs(dog_y - skul_y)
	cat_skul_dist_x = abs(cat_x - skul_x)
	dog_skul_dist_x = abs(dog_x - skul_x)
	catAction = getPlayerAction(cat_y,coin_y,skul_y,cat_skul_dist_x,cat_coin_dist,cat_skul_dist)
	dogAction = getPlayerAction(dog_y,coin_y,skul_y,dog_skul_dist_x,dog_coin_dist,dog_skul_dist)
	dogA = random.randint(0,4)
	catA = random.randint(0,4)
	messageA = str(actions[dogA] + " " + actions[catA])
	#return messageA
	time.sleep(0.0001)  # calculate time 
	return catAction + " " + dogAction





def skipframe():
	socket.send_string("req_skip")
	poller = zmq.Poller()
	poller.register(socket, zmq.POLLIN)
	evt = dict(poller.poll(TIMEOUT))
	if evt:
		if evt.get(socket) == zmq.POLLIN:
			response = socket.recv(zmq.NOBLOCK).decode("utf-8")
			#print(response)
			return 

isStart = False
frameNum_prev = 0
while True:
	#skipframe()
	#time.sleep(0.0001)	
	socket.send_string(action)
	poller = zmq.Poller()
	poller.register(socket, zmq.POLLIN)
	evt = dict(poller.poll(TIMEOUT))
	if evt:
		if evt.get(socket) == zmq.POLLIN:
			response = socket.recv(zmq.NOBLOCK).decode("utf-8")
			if response == "res_skip":
				continue
			states = np.array(list(map(lambda x:float(x),response.split(" "))))
			frameNum = int(states[0])
			if not isStart:
				frameNum_prev = frameNum-1
				isStart = True
			if frameNum_prev == frameNum:
				frameNum_prev = frameNum
				continue

			if len(states_hist) > 10000:
				states_hist.clear()
			states_hist.append(states)
			action = getAction(states)
			print(list(map(lambda x : round(x,2),states))+[action])
			frameNum_prev = frameNum
			continue
	print("no evt")
	time.sleep(0.5)
	socket.close()
	socket = context.socket(zmq.REQ)
	socket.connect("tcp://localhost:12346")