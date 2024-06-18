FROM node:8.15.1-jessie

WORKDIR /Area/Area.MobileClient/bin/Debug

COPY client.apk ./public

WORKDIR /web

COPY . .

RUN npm install

EXPOSE 8081

CMD ["npm", "run", "dev"]