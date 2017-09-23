extends Node2D

var connected = false
var stream

func _ready():
	set_process(true)

func _process(delta):
	if stream.get_available_bytes() > 0:
		var resp = stream.get_u8()
		print("fromserver: ", resp)

func init_game():
	stream = StreamPeerTCP.new()
	stream.connect("127.0.0.1", 44491)
	stream.put_u8(0x12)
	var name = "PlayerName"
	var nameBytes = name.to_utf8()
	stream.put_32(nameBytes.size())
	stream.put_data(nameBytes)
	#stream.put_utf8_string(name)
	var ok = stream.get_u8()
	if ok == 0x2:
		print("OK")
	var conplayer = stream.get_u8()
	if conplayer == 0x15:
		var clientIdSize = stream.get_32()
		var clientId = stream.get_32()
		print("client ID connected: ", clientId)
		var conplayername = stream.get_u8()
		var namesize = stream.get_32()
		var conname = stream.get_string(namesize)
		print("name connected: ", conname)
	