FROM mcr.microsoft.com/dotnet/aspnet:8.0

#Asp.Net Core Docker基于Debian发行版，镜像源已经从wheezy升级到stretch。
#如果在阿里云、腾讯云上部署，不设置镜像源，直接访问国外服务器，速度也很快。

#RUN echo "deb http://mirrors.aliyun.com/debian stretch main contrib non-free \
#deb-src http://mirrors.aliyun.com/debian stretch main contrib non-free \
#deb http://mirrors.aliyun.com/debian stretch-updates main contrib non-free \
#deb-src http://mirrors.aliyun.com/debian stretch-updates main contrib non-free \
#deb http://mirrors.aliyun.com/debian-security stretch/updates main contrib non-free \
#deb-src http://mirrors.aliyun.com/debian-security stretch/updates main contrib non-free \
#deb http://mirrors.aliyun.com/debian/ stretch-backports main non-free contrib \
#deb-src http://mirrors.aliyun.com/debian/ stretch-backports main non-free contrib" > /etc/apt/sources.list
#
#
##必须安装System.Drawing.Common依赖库
##RUN apt-get update && apt-get install libfontconfig1 -y
##RUN apt-get update && apt-get install libgdiplus -y && ln -s libgdiplus.so gdiplus.dll
#RUN sed -i "s@archive.ubuntu.com/@/mirrors.tuna.tsinghua.edu.cn/@g" /etc/apt/sources.list\ && rm -Rf /var/lib/apt/lists/* \
#RUN ln -s /lib/x86_64-linux-gnu/libdl-2.24.so /lib/x86_64-linux-gnu/libdl.so
#
##此行是关于画心电图
##COPY ./fonts/msyh.ttc /usr/share/fonts/dejavu
#
WORKDIR /app
COPY . /app
RUN mkdir -p /app/data
EXPOSE 5000
ENTRYPOINT ["dotnet", "ChatEx.dll"]