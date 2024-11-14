FROM node:21-alpine AS builder

WORKDIR /app
COPY ["Blog.WebUi/package.json", "./"]
COPY ["Blog.WebUi/package-lock.json", "./"]

RUN npm install
RUN npm install -g @angular/cli

COPY ["Blog.WebUi", "./"]

EXPOSE 4200
CMD ["ng", "serve", "--host", "0.0.0.0", "--configuration", "production", "--port", "80" ]