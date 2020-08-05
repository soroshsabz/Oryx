ARG BASE_IMAGE
FROM ${BASE_IMAGE}

COPY --from=supportfilesimage /tmp/oryx/ /tmp/oryx

RUN apt-get update \
	&& apt-get upgrade -y \
	&& apt-get install -y --no-install-recommends \
		xz-utils \
	&& rm -rf /var/lib/apt/lists/*

