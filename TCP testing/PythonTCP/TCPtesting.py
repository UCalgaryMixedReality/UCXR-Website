import socket
import time

# Define server address and port
HOST = '127.0.0.1'  # The server's IP address
PORT = 12345        # The server's port number

# Create a socket object
client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Connect to the server
client_socket.connect((HOST, PORT))

# Send coordinates to the server
for i in range(50000):  # Send coordinates five times (for demonstration)
    # Simulate generating coordinates (X and Y values)
    x = i * 10
    y = i * 20

    # Construct message containing coordinates
    message = f"X:{x},Y:{y}"

    # Send message to the server
    client_socket.sendall(message.encode())

    # Wait for a short time before sending the next set of coordinates
    time.sleep(1)

# Close the connection
client_socket.close()
