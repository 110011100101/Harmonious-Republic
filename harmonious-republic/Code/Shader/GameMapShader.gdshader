shader_type canvas_item;

uniform int index;
uniform float alpha;

void fragment() {
    // 下层暗，上层原色
    float factor = 0.4 + 0.6 * (float(index) / 4.0);
    COLOR.rgb *= factor;
	COLOR.a = alpha;
}
