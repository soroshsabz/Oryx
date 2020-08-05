ARG DEBIAN_FLAVOR
FROM buildpack-deps:${DEBIAN_FLAVOR}-curl as main

RUN apt-get update \
	&& apt-get upgrade -y \
	&& apt-get install -y --no-install-recommends \
		xz-utils \
		unzip \
	&& rm -rf /var/lib/apt/lists/*

COPY --from=support-files-image-for-build /tmp/oryx/ /tmp/oryx
