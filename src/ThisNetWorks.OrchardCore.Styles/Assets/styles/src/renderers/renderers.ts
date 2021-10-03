/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types*/
export declare type fEmptyVoid = () => void;
export declare type fEmptyReturn = () => any;
export declare type fArgVoid = (...args: any[]) => void;
export declare type fArgReturn = (...args: any[]) => any;
export declare type fArgReturnPromise = (...args: any[]) => Promise<any>;
export declare type fArgReturnString = (...args: any[]) => string;

import dashify from "dashify";
import { Liquid } from "liquidjs";
import postcss from "postcss";
import autoprefixer from "autoprefixer";

const kebabCase = (model: any): any => {
  const m = { ...model };
  m.kebabName = dashify(model.name);
  return m;
};

const renderers: Record<string, fArgReturnPromise> = {
  CssVarColor: (model: any): Promise<any> => {
    const m = kebabCase(model);
    return Promise.resolve(
      `--${m.kebabName}-hex: ${m.value.hex};
--${m.kebabName}-rgb: ${m.value.rgb};`
    );
  },
  RootVars: (model: any): Promise<any> => {
    const m = kebabCase(model);
    const result = m.value.join("\n");
    return Promise.resolve(`:root {
          ${result}
        }
        `);
  },
  CssHexEntry: (model: any): Promise<any> => {
    const m = kebabCase(model);
    return Promise.resolve(`${m.kebabName}: ${m.value.hex};`);
  },
  CssRgbEntry: (model: any): Promise<any> => {
    const m = kebabCase(model);
    return Promise.resolve(`${m.kebabName}: ${m.value.rgb};`);
  },
  CssEntry: (model: any): Promise<any> => {
    const m = kebabCase(model);
    return Promise.resolve(`${m.kebabName}: ${m.value};`);
  },
  CssVarEntry: (model: any): Promise<any> => {
    const m = kebabCase(model);
    return Promise.resolve(`--${m.kebabName}: ${m.value};`);
  },
  CssSizeEntry: (model: any): Promise<any> => {
    const m = kebabCase(model);
    return Promise.resolve(`${m.kebabName}: ${m.value.size}${m.value.unit};`);
  },
  CssVarSizeEntry: (model: any): Promise<any> => {
    const m = kebabCase(model);
    return Promise.resolve(`--${m.kebabName}: ${m.value.size}${m.value.unit};`);
  },
  CssUrlEntry: (model: any): Promise<any> => {
    const m = kebabCase(model);
    return Promise.resolve(`${m.kebabName}: url("${m.value}");`);
  },
  LiquidJs: (template: string, model: any): Promise<any> => {
    const m = kebabCase(model);
    const engine = new Liquid();
    return engine.parseAndRender(template, m);
  },
};

const styleRenderer = (renderer: any, model: any): Promise<any> => {
  if (typeof renderer === "object") {
    const rendererName = renderer.name;
    const renderFn = renderers[rendererName];
    return postRender(renderFn(renderer.template, model));
  } else {
    const rendererName = renderer;
    const renderFn = renderers[rendererName];
    return postRender(renderFn(model));
  }
};

const postRender = (render: Promise<any>): Promise<any> =>
  render.then((result: any) =>
    postcss([autoprefixer]).process(result, { from: undefined })
  );

export { styleRenderer };
