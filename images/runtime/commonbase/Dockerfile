ARG DEBIAN_FLAVOR
FROM buildpack-deps:${DEBIAN_FLAVOR}-curl as main
RUN apt-get update \
	&& apt-get upgrade -y \
	&& apt-get install -y --no-install-recommends \
		xz-utils \
		unzip \
	&& rm -rf /var/lib/apt/lists/*

FROM debian:buster-slim as tempFiles
COPY --from=startupCmdGen /opt/startupcmdgen/startupcmdgen /opt/startupcmdgen/startupcmdgen
COPY --from=support-files-image-for-build /tmp/oryx/ /opt/tmp

FROM main
COPY --from=tempFiles /opt /opt
