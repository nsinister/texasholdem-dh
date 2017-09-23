extends Node2D

func _ready():
	set_process(true)
	get_node("game").init_game()
	get_node("game").show()
	
func _process(delta):
	pass