FROM node:20-alpine AS builder

WORKDIR /app
COPY package.json .
COPY package-lock.json .

RUN npm install
RUN npm install -g @angular/cli

COPY . .

RUN ng build --configuration production

CMD node dist/blog.web-ui/server/server.mjs