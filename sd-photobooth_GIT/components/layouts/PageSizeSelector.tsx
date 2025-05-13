interface PageSizeSelectorProps {
  value: number;
  onChange: (value: number) => void;
}

export const PageSizeSelector = ({
  value,
  onChange,
}: PageSizeSelectorProps) => {
  return (
    <div className="flex items-center justify-end gap-2">
      <label>Rows per page:</label>
      <select
        className="border px-2 py-1 rounded"
        value={value}
        onChange={(e) => onChange(Number(e.target.value))}
      >
        {[5, 10, 20, 50].map((size) => (
          <option key={size} value={size}>
            {size}
          </option>
        ))}
      </select>
    </div>
  );
};
