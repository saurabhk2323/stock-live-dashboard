FROM node:20-alpine
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
EXPOSE 3000
CMD [ "npm", "start" ]


# production build
# # Build Stage
# FROM node:16-alpine AS build
# WORKDIR /app
# COPY . .
# RUN npm install && npm run build

# # Serve Stage
# FROM nginx:alpine
# COPY --from=build /app/build /usr/share/nginx/html
# EXPOSE 80