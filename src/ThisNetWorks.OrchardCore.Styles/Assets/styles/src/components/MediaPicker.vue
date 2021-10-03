<template>
  <div>
    <div class="form-control-plaintext media-field-toolbar" v-cloak>
      <div class="btn-group">
        <button
          type="button"
          class="btn btn-secondary btn-sm"
          :disabled="!mediaItem"
          v-on:click="setMediaFieldPrefs"
        >
          <span v-show="!smallThumbs" title="Small Thumbs"
            ><i class="fas fa-compress-alt" aria-hidden="true"></i
          ></span>
          <span v-show="smallThumbs" title="Large Thumbs"
            ><i class="fas fa-expand-alt" aria-hidden="true"></i
          ></span>
        </button>
        <a
          v-show="canAddMedia"
          v-on:click="showModal"
          class="btn btn-secondary btn-sm"
          ><i class="fas fa-plus" aria-hidden="true"></i
        ></a>
        <a
          v-on:click="removeSelected"
          class="btn btn-secondary btn-sm"
          v-bind:class="{ disabled: !selectedMedia }"
          ><i class="fas fa-trash-alt" aria-hidden="true"></i
        ></a>
      </div>
      <!-- <label class="selected-media-name">
            <code class="text-right" v-if="selectedMedia">@T["{{{{ selectedMedia.name }}}}{{{{ selectedMedia.mediaText === '' ? '' : ', ' + selectedMedia.mediaText }}}} ({{{{ isNaN(fileSize)? 0 : fileSize }}}} KB)"]</code>
        </label> -->
    </div>

    <media-field-thumbs-container
      :media-items="mediaItems"
      :thumb-size="thumbSize"
      :selected-media="selectedMedia"
      :id-prefix="idPrefix"
      allow-anchors="false"
    ></media-field-thumbs-container>

    <div class="modal" ref="mediaModal">
      <div class="modal-dialog modal-lg media-modal" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Select Media</h5>
            <button
              type="button"
              class="close"
              data-dismiss="modal"
              aria-label="Close"
            >
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body"></div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-primary mediaFieldSelectButton"
            >
              OK
            </button>
            <button
              type="button"
              class="btn btn-secondary"
              data-dismiss="modal"
            >
              Cancel
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
import { Component, Prop, Vue, Emit } from "vue-property-decorator";
import * as jquery from "jquery";
import axios from "axios";

interface IMediaFieldPrefs {
  smallThumbs: boolean;
}

interface IMediaAnchor {
  x: number;
  y: number;
}

interface IMediaItem {
  name: string;
  size: number;
  lastModify: number;
  folder: string;
  url: string;
  mediaPath: string;
  mime: string;
  mediaText: string;
  anchor: IMediaAnchor;
}

const $ = jquery as any;

@Component({ name: "media-picker" })
export default class MediaPicker extends Vue {
  @Prop() private path!: string;
  @Prop() private mediaItemUrl!: string;

  private mediaItem: IMediaItem | null = null;
  private selectedMedia: IMediaItem | null = null;
  private mediaFieldPrefs: IMediaFieldPrefs = { smallThumbs: false };

  get mediaItems(): IMediaItem[] {
    if (this.mediaItem) {
      return [this.mediaItem];
    } else {
      return [];
    }
  }

  get canAddMedia(): boolean {
    return !this.mediaItem;
  }

  get thumbSize(): number {
    return this.mediaFieldPrefs.smallThumbs ? 120 : 240;
  }

  get smallThumbs(): boolean {
    return this.mediaFieldPrefs.smallThumbs;
  }

  get idPrefix(): string {
    return "xys";
  }

  showModal() {
    // eslint-disable-next-line @typescript-eslint/no-this-alias
    const self = this;
    if (!this.mediaItem) {
      $("#mediaApp")
        .detach()
        .appendTo($(this.$refs.mediaModal).find(".modal-body"));
      $("#mediaApp").show();
      const modal = $(this.$refs.mediaModal).modal();
      $(this.$refs.mediaModal)
        .find(".mediaFieldSelectButton")
        .off("click")
        .on("click", () => {
          // self.addMediaFiles(mediaApp.selectedMedias);
          const selectedItems = (window as any).mediaApp.selectedMedias;
          if (selectedItems?.length > 0) {
            if (selectedItems.length > 1) {
              alert("Only one item is allowed on this style");
            }
            self.mediaItem = selectedItems[0];
            self.pathChange();
            //[image]' + mediaApp.selectedMedias[i].mediaPath + '[/image]
          }

          // we don't want the included medias to be still selected the next time we open the modal.
          (window as any).mediaApp.selectedMedias = [];

          modal.modal("hide");
          return true;
        });
    }
  }

  @Emit()
  pathChange(): string {
    if (!this.mediaItem?.mediaPath) {
      return "";
    }

    return this.mediaItem.mediaPath;
  }

  @Emit()
  onError(error: any) {
    return error;
  }

  mounted(): void {
    const mediaFieldPrefs = localStorage.getItem("mediaFieldPrefs");
    if (mediaFieldPrefs) {
      this.mediaFieldPrefs = JSON.parse(mediaFieldPrefs) as IMediaFieldPrefs;
    }

    // this.$on("selectAndDeleteMediaRequested", (media) => {
    //   this.selectAndDeleteMedia(media);
    // });

    this.$on("selectMediaRequested", (media: IMediaItem) => {
      this.selectMedia(media);
    });

    // this.$on("filesUploaded", (files) => {
    //   this.addMediaFiles(files);
    // });

    if (this.path?.length > 0) {
      axios
        .get(`${this.mediaItemUrl}?path=${encodeURIComponent(this.path)}`)
        .then((response) => {
          this.mediaItem = response.data as IMediaItem;
        })
        .catch((e) => {
          console.log(e);
        });
    }
  }

  private setMediaFieldPrefs() {
    this.mediaFieldPrefs.smallThumbs = !this.mediaFieldPrefs.smallThumbs;
    localStorage.setItem(
      "mediaFieldPrefs",
      JSON.stringify(this.mediaFieldPrefs)
    );
  }

  selectMedia(media: IMediaItem) {
    this.selectedMedia = media;
  }

  removeSelected() {
    this.mediaItem = null;
    this.selectedMedia = null;
  }

  // selectAndDeleteMedia(media) {
  //   this.selectedMedia = media;
  //   // setTimeout because sometimes removeSelected was called even before the media was set.
  //   setTimeout(function () {
  //     this.removeSelected();
  //   }, 100);
  // }
}
</script>

<style lang="css" />
