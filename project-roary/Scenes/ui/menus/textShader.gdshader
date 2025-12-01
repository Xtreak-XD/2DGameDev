shader_type canvas_item;

uniform float height = 10.0;
uniform float frequency = 2.0;
uniform float speed = 5.0;

void vertex() {
    // Displace the Y coordinate using a sine wave
    // UV.x makes the wave go across the text from left to right
    // TIME makes the wave move over time
    VERTEX.y += height * sin((frequency * UV.x) + (speed * TIME));
}
