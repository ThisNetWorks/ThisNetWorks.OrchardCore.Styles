interface IDescriptor {
  displayText: string;
  value: string;
}

/**
* Helper to produce an array of enum descriptors.
* @param enumeration Enumeration object.
* @param separatorRegex Regex that would catch the separator in your enum key
.*/
const enumToDescriptedArray = <T>(
  enumeration: T,
  separatorRegex = /_/g
): IDescriptor[] => {
  const t = (Object.keys(enumeration) as Array<keyof T>)
    .filter((key) => isNaN(Number(key)))
    .filter(
      (key) =>
        typeof enumeration[key] === "number" ||
        typeof enumeration[key] === "string"
    )
    .map((key) => ({
      value: String(enumeration[key]),
      displayText: String(key).replace(separatorRegex, " "),
    }));

  return t;
};

export { IDescriptor, enumToDescriptedArray };
