FROM php-7.4
SHELL ["/bin/bash", "-c"]

ENV APACHE_RUN_USER="www-data" \
    APACHE_DOCUMENT_ROOT="/home/site/wwwroot" \
    APACHE_PORT="8080"

COPY installOryxPrereqs.sh /tmp/

RUN chmod +x /tmp/installOryxPrereqs.sh \
    && /tmp/installOryxPrereqs.sh
