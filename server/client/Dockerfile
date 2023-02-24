### STAGE 1: Build ###
FROM node:16.10-alpine AS build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build
### STAGE 2: Run ###
FROM nginx:1.17.1-alpine
COPY /nginx-custom.conf /etc/nginx/conf.d/default.conf
COPY --from=build /usr/src/app/dist/client-application /usr/share/nginx/html

# Copy the EntryPoint
COPY ./appEntryPoint.sh /
RUN chmod +x appEntryPoint.sh
ENTRYPOINT ["sh","/appEntryPoint.sh"]
CMD ["nginx", "-g", "daemon off;"]