shader_type canvas_item;

uniform float level_diff;

void vertex() {
	float color = 1.0 - level_diff / 5.0;
	COLOR = vec4(color, color, color, (1.0 + level_diff) * 3.0);
}

void fragment() {
}
