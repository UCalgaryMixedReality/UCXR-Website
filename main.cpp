#include <iostream>
#include <glad/glad.h>
#include <GLFW\glfw3.h>
using namespace std;

int main()
{
	glfwInit(); //intialize

	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	GLFWwindow* window = glfwCreateWindow(800, 800, "Window_Name", NULL, NULL); //width, height, window name, fullscreen/not full screen, not important
	if (window == NULL)
	{
		cout << "Failed to create GLFW window" << endl; // error checking if window fails to create
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window);

	while (!glfwWindowShouldClose(window))
	{
		glfwPollEvents(); // process pooled events
	}

	glfwDestroyWindow(window);
	glfwTerminate(); //terminate
	return 0;
}