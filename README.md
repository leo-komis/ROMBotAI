# ROMBot-AI

[![Deploy to Azure App Service](https://github.com/leo-komis/ROMBot-AI/actions/workflows/azure_deploy.yml/badge.svg)](https://github.com/leo-komis/ROMBot-AI/actions/workflows/azure_deploy.yml)

## Introduction

The code is a program that creates a chatbot. A chatbot is a computer program that is designed to interact with people in a conversational manner. The chatbot in this code is capable of answering questions. It has a memory of previous answers, and if it receives a question that it has already answered, it can quickly retrieve the answer from its memory. If it receives a new question, it can generate an answer using the OpenAI API.

The program is organized in a way that makes it easy to use and understand. It has a number of methods that allow you to interact with the chatbot, such as retrieving answers and storing answers and their related information. Additionally, the program uses a number of advanced techniques to generate answers, such as generating embeddings for answers, which are mathematical representations of the answers that capture their meaning in a high-dimensional space.

Overall, the program is designed to provide a conversational experience for users. It is capable of understanding questions and generating answers, making it an ideal solution for those who want to create a chatbot that can interact with users in a natural and intuitive manner.

The program also includes a training mode, where it trains a text classification model using TensorFlow on the list of question-answer pairs. This model can then be used to improve the accuracy of the program's answer generation over time. The program is deployed as an Azure web app and includes a simple HTML interface for users to enter their questions.

## Technicalities

The code is a C# program that implements a chatbot that can answer questions. As mentioned before, it uses the OpenAI API to generate embeddings for the answers and to generate answers to questions. The program is structured as a static class called "Program" that contains various methods and fields.

The fields in the class include:

* "embeddings", a dictionary that maps answer strings to arrays of floats that represent their embeddings.
* "qaPairs", a list of string arrays, each of which contains a question and its answer.
* "chatBot", an instance of the ChatBot class, which is used to generate answers to questions.

The methods in the class include:

* "LoadEmbeddingsAsync", which loads the answer embeddings from a file called "embeddings.csv".
* "GetChatBot", which returns the current instance of the ChatBot class.
* "SetChatBot", which sets the current instance of the ChatBot class.
* "GetAnswerAsync", which retrieves the answer to a given question. If the answer is already in the "embeddings" dictionary, it is returned directly. Otherwise, the answer is generated using the OpenAI API and then added to the "embeddings" dictionary.
* "GenerateEmbeddingAsync", which generates an embedding for a given text string using the OpenAI API.
* "GetClosestEmbedding", which finds the answer string in the "embeddings" dictionary that is most similar to a given question string.
* "CosineSimilarity", which calculates the cosine similarity between two embeddings in high-dimensional space.

The code also includes several using statements at the top that bring in various namespaces from the .NET framework and other libraries. These are used throughout the program to access various classes and methods.


## Further ideas to develop the program:

### Improve the chatbot's response: 
The current implementation simply selects the closest embedding to the input question, but you could explore more sophisticated approaches like natural language processing, machine learning models, or integrating other APIs like Dialogflow or Wit.ai to generate more accurate and natural responses.

### Implement authentication: 
The web app currently does not require any authentication, which could be a potential security risk. You could implement authentication using OAuth or other authentication protocols to ensure that only authorized users can access the app.

### Integrate with other services: 
You could integrate the web app with other services like a database, a messaging service, or a notification service to provide more functionality and improve the user experience.

### Improve the training process: 
The current implementation trains the model using a simple binary classification approach, but you could explore other training methods like transfer learning, reinforcement learning, or unsupervised learning to improve the accuracy and efficiency of the training process.

### Expand the scope: 
The current implementation only answers questions related to a specific domain, but you could expand the scope of the program to cover multiple domains and provide more comprehensive answers to a wider range of questions.
