# -*- coding: utf-8 -*-
"""
Created on Sat Apr  6 11:28:56 2024

@author: LENOVO
"""

# Creating a rest API to fetch top 10 search info from the second layer

# Import the Flask and jsonify classes from the Flask module
from flask import Flask, jsonify
import json

# Create a Flask application instance
app = Flask(__name__)


# Define a single route to fetch JSON data from a file
@app.route('/get_data', methods=['GET'])
def get_data():
    try:
        # Assuming your JSON file is named 'data.json' and located in the same directory as this script
        with open('data.json', 'r') as file:
            data = json.load(file)
        return jsonify(data), 200
    except FileNotFoundError:
        return jsonify({'error': 'Data file not found'}), 404
    except Exception as e:
        return jsonify({'error': str(e)}), 500

if __name__ == '__main__':
    app.run(debug=True)
