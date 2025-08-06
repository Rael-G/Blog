FROM node:20-alpine AS builder

WORKDIR /app
COPY Blog.WebUi/package.json .
COPY Blog.WebUi/package-lock.json .

RUN npm install
RUN npm install -g @angular/cli

COPY Blog.WebUi/. .

RUN ng build --configuration production

CMD node dist/blog.web-ui/server/server.mjs