extends Node2D

# class member variables go here, for example:
# var a = 2
# var b = "textvar"

func _ready():
	# Called every time the node is added to the scene.
	# Initialization here
	var ch = get_node("chat_panel/lb_chat_history")
	ch.set_scroll_follow(true)
	
func _on_le_chat_message_text_entered( text ):
	var ch = get_node("chat_panel/lb_chat_history")
	ch.set_text(ch.get_text() + "\n" + text)
	var le = get_node("chat_panel/le_chat_message")
	le.clear()