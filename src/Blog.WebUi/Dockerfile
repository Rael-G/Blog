FROM node:lts-alpine AS builder

WORKDIR /app
COPY ["src/Blog.WebUi/package.json", "./"]
COPY ["src/Blog.WebUi/package-lock.json", "./"]

RUN npm install
RUN npm install -g @angular/cli

COPY ["src/Blog.WebUi", "./"]

EXPOSE 4200
CMD ["ng", "serve", "--host", "0.0.0.0", "--configuration", "production", "--port", "80" ]