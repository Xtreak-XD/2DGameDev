shader_type canvas_item;

uniform float float_strength : hint_range(0.0, 50.0) = 10.0;
uniform float float_speed : hint_range(0.0, 10.0) = 1.0;

void fragment() {
    float offSetY = float_strength * sin(TIME * float_speed);
	COLOR = texture(TEXTURE, UV + vec2(0.0, offSetY) / 500.0);
}